import * as ko from "knockout";

import * as Enums from "../models/enums"
import { Loader as VpGame } from "../api/vpGame";
import Timespan from "../shared/timespan"
import MatchViewModel from "../viewModels/matchViewModel";

interface ICategoryFilter { [c: string]: boolean }

export default class MainViewModel {
    private readonly matches: KnockoutObservableArray<MatchViewModel>;
    private readonly filteredMatches: KnockoutObservableArray<MatchViewModel>;
    private readonly clock: KnockoutObservable<string>;
    private readonly timeSinceLastUpdate: KnockoutObservable<string>;
    private readonly filter: ICategoryFilter;
    private lastRefresh: Date;

    constructor() {
        this.matches = ko.observableArray<MatchViewModel>();
        this.filteredMatches = ko.observableArray<MatchViewModel>();
        this.clock = ko.observable<string>();
        this.timeSinceLastUpdate = ko.observable<string>();
        this.lastRefresh = new Date();
        this.filter = {};

        this.refreshClock();
        this.refreshTimeSinceLastUpdate();

        this.initFilter();

        this.startClock();
        this.startMatchLoading();
    }

    public applyCategoryFilter(parent: MainViewModel) {
        localStorage.setItem("filter", JSON.stringify(parent.filter));

        let filteredMatches = new Array<MatchViewModel>();

        parent.filteredMatches.removeAll();
        parent.matches().forEach(m => {

            // dont show matches which started over 5Minutes ago
            if (m.startsIn < -Timespan.fromMinutes(5)) {
                return;
            }

            // filter by category
            if (parent.filter[Enums.Category[m.category]]) {
                filteredMatches.push(m);
            }
        });

        parent.filteredMatches(filteredMatches.sort((a, b) => a.startsIn - b.startsIn));
    }

    public getImagePath(category: Enums.Category): string {
        const name = Enums.Category[category].toLowerCase();
        return `/img/category/${name}_64.png`;
    }

    private loadMatches(): void {
        VpGame.getMatchesAsync().then(matches => {
            this.matches(matches.map(m => new MatchViewModel(m)))

            this.applyCategoryFilter(this);

            this.lastRefresh = new Date();
        });
    }

    private initFilter(): void {
        this.filter[Enums.Category[Enums.Category.Basketball]] = true;
        this.filter[Enums.Category[Enums.Category.Csgo]] = true;
        this.filter[Enums.Category[Enums.Category.Dota2]] = true;
        this.filter[Enums.Category[Enums.Category.Soccer]] = true;
        this.filter[Enums.Category[Enums.Category.Tennis]] = true;

        const storedFilterStr = localStorage.getItem("filter");
        if (storedFilterStr) {
            const storedFilter = JSON.parse(storedFilterStr);

            for (let s in storedFilter) {
                if (this.filter.hasOwnProperty(s)) {
                    this.filter[s] = storedFilter[s];
                }
            }
        }
    }

    private startMatchLoading(): void {
        this.loadMatches();

        setInterval(() => {
            this.loadMatches();
        }, Timespan.fromSeconds(30));
    }

    private startClock(): void {
        setInterval(() => {
            this.refreshClock();
            this.refreshTimeSinceLastUpdate();
            this.refreshMatchStartDate();

        }, Timespan.fromSeconds(1));
    }

    private refreshClock() {
        const date = new Date();
        const hours = ("0" + date.getHours()).slice(-2);
        const minutes = ("0" + date.getMinutes()).slice(-2);
        const seconds = ("0" + date.getSeconds()).slice(-2);

        this.clock(hours + ":" + minutes + ":" + seconds);
    }

    private refreshTimeSinceLastUpdate() {
        const refreshAgo = new Date().getTime() - this.lastRefresh.getTime();
        const refreshMinutes = Math.floor(refreshAgo / 60000);
        const refreshSeconds = ("0" + Math.floor((refreshAgo - (refreshMinutes * 60000)) / 1000)).slice(-2);

        this.timeSinceLastUpdate(`${refreshMinutes}:${refreshSeconds}`);
    }

    private refreshMatchStartDate() {
        this.matches().forEach(m => {
            m.refreshStartDateFormatted();
        });
    }
}