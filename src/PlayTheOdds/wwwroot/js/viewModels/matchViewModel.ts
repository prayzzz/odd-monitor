import * as ko from "knockout";

import * as Enums from "../models/enums";
import { Match, Team } from "../models/models"
import Timespan from "../shared/timespan"
import WagerViewModel from "./wagerViewModel"

export default class MatchViewModel {
    private nextWagerStart: Date;

    public readonly startDateFormatted: KnockoutObservable<string>;
    public readonly match: Match;
    public readonly wagers: WagerViewModel[];
    public readonly filteredWagers: KnockoutObservableArray<WagerViewModel>;

    constructor(match: Match) {
        this.match = match;
        this.wagers = match.wagers.map(m => new WagerViewModel(m, match.teamLeft, match.teamRight));

        this.filteredWagers = ko.observableArray<WagerViewModel>();
        this.startDateFormatted = ko.observable<string>();

        this.wagers
            .filter(w => w.wager.status === Enums.WagerStatus.Open || w.wager.status === Enums.WagerStatus.Live)
            .forEach(w => {
                this.filteredWagers.push(w)
            });

        this.nextWagerStart = this.getClosestWagerStartDate();
        this.refreshStartDateFormatted();
    }

    public get tournamentName(): string {
        return this.match.tournamentName;
    }

    public get category(): Enums.Category {
        return this.match.category;
    }

    public get teamLeft(): Team {
        return this.match.teamLeft;
    }

    public get teamRight(): Team {
        return this.match.teamRight;
    }

    public get matchLink(): string {
        return this.match.matchLink;
    }

    public get matchFormat(): string {
        return Enums.MatchFormat[this.match.matchFormat].toUpperCase();
    }

    public get isLive(): boolean {
        const current = new Date().getTime();
        const match = this.nextWagerStart.getTime();

        return match < current;
    }

    /**
     * Time to start in milliseconds
     */
    public get startsIn(): number {
        return this.nextWagerStart.getTime() - new Date().getTime();
    }

    /**
     * Time to start in in mm:ss format
     */
    public get startsInFormatted(): string {
        const minutes = Math.floor(this.startsIn / 60000);
        const seconds = ("0" + Math.floor((this.startsIn - (minutes * 60000)) / 1000)).slice(-2);

        return `in ${minutes}:${seconds}`;
    }

    public refreshStartDateFormatted() {
        if (this.isLive) {
            this.startDateFormatted("Live");
            return;
        }

        if (this.startsIn > Timespan.fromMinutes(30)) {
            // set absolute time        
            const hours = ("0" + this.nextWagerStart.getHours()).slice(-2);
            const minutes = ("0" + this.nextWagerStart.getMinutes()).slice(-2);
            this.startDateFormatted(hours + ":" + minutes);
            return;
        }

        // set relative time            
        const minutes = Math.floor(this.startsIn / 60000);
        const seconds = ("0" + Math.floor((this.startsIn - (minutes * 60000)) / 1000)).slice(-2);

        this.startDateFormatted(`in ${minutes}:${seconds}`);
        return;
    }

    private getClosestWagerStartDate(): Date {
        if (this.wagers.length == 0) {
            return this.match.startDate;
        }

        let now = new Date();
        let date = this.wagers[this.wagers.length - 1].wager.startDate;
        this.filteredWagers().forEach(w => {
            if (w.wager.startDate > now && w.wager.startDate < date) {
                date = w.wager.startDate;
            }
        })

        return date;
    }
}