import * as ko from "knockout";

import { VpGame } from "../odds/vpGame"
import Ajax from "../shared/ajax"
import Timespan from "../shared/timespan"
import * as Enums from "../enums/enums"
import MatchViewModel from "../viewModels/matchViewModel";

interface ICategoryFilter { [c: string]: boolean }

export default class MainViewModel {
    private readonly vpGame = new VpGame();
    private readonly matches: KnockoutObservableArray<MatchViewModel>;
    private readonly filteredMatches: KnockoutObservableArray<MatchViewModel>;
    private readonly time: KnockoutObservable<string>;
    private readonly filter: ICategoryFilter;

    constructor() {
        this.matches = ko.observableArray<MatchViewModel>();
        this.filteredMatches = ko.observableArray<MatchViewModel>();
        this.time = ko.observable<string>();
        this.filter = {};

        this.initFilter();

        this.startClock();
        this.startHeartbeat();
        this.startMatchLoading();
    }

    public filterChanged(parent: MainViewModel) {
        localStorage.setItem("filter", JSON.stringify(parent.filter));

        parent.filteredMatches.removeAll();
        parent.matches().forEach(m => {

            // dont show matches which started over 5Minutes ago
            if (m.startsIn < -Timespan.fromMinutes(5)) {
                return;
            }

            // filter by category
            if (parent.filter[Enums.Category[m.category]]) {
                parent.filteredMatches.push(m);
            }
        });
    }

    public getImagePath(category: Enums.Category): string {
        const name = Enums.Category[category].toLowerCase();
        return `/img/category/${name}_64.png`;
    }

    private loadMatches(): void {
        this.vpGame.getMatchesAsync().then(matches => {
            // clear current
            this.matches().forEach(m => m.dispose());
            this.matches.removeAll();

            // push new
            matches.forEach(m => this.matches.push(new MatchViewModel(m)));

            // apply filter
            this.filterChanged(this);
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
        }, Timespan.fromMinutes(2));
    }

    private startHeartbeat(): void {
        Ajax.get("/api/v1/heartbeat").send();

        setInterval(() => {
            Ajax.get("/api/v1/heartbeat").send();
        }, Timespan.fromMinutes(1));
    }

    private startClock(): void {
        setInterval(() => {
            const date = new Date();
            const hours = ("0" + date.getHours()).slice(-2);
            const minutes = ("0" + date.getMinutes()).slice(-2);
            const seconds = ("0" + date.getSeconds()).slice(-2);

            this.time(hours + ":" + minutes + ":" + seconds);
        }, Timespan.fromSeconds(1));
    }
}