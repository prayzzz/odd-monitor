import * as ko from "knockout";

import * as Enums from "../models/enums";
import { Match, Team } from "../models/models"
import Timespan from "../shared/timespan"
import WagerViewModel from "./wagerViewModel"

export default class MatchViewModel {
    private countdownHandle: number;
    public readonly formattedDate: KnockoutObservable<string>;
    public readonly match: Match;
    public readonly wagers: WagerViewModel[];
    public readonly filteredWagers: KnockoutObservableArray<WagerViewModel>;

    constructor(match: Match) {
        this.match = match;
        this.wagers = match.wagers.map(m => new WagerViewModel(m, match.teamLeft, match.teamRight));

        this.filteredWagers = ko.observableArray<WagerViewModel>();
        this.formattedDate = ko.observable<string>();

        this.initFormattedDate();

        this.wagers
            .filter(w => w.wager.status === Enums.WagerStatus.Open || w.wager.status === Enums.WagerStatus.Live)
            .forEach(w => {
                this.filteredWagers.push(w)
            });
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

    /**
     * Time to start in milliseconds
     */
    public get startsIn(): number {
        return this.match.startDate.getTime() - new Date().getTime();
    }

    /**
     * Time to start in in mm:ss format
     */
    public get startsInFormatted(): string {
        const minutes = Math.floor(this.startsIn / 60000);
        const seconds = ("0" + Math.floor((this.startsIn - (minutes * 60000)) / 1000)).slice(-2);

        return `in ${minutes}:${seconds}`;
    }

    public get isLive(): boolean {
        const current = new Date().getTime();
        const match = this.match.startDate.getTime();

        return match < current;
    }

    public dispose(): void {
        clearInterval(this.countdownHandle);
    }

    private initFormattedDate() {
        if (this.isLive) {
            this.formattedDate("Live");
            return;
        }

        // Show countdown, if starts in under 30Minutes
        if (this.startsIn < Timespan.fromMinutes(30)) {
            this.startCountdown();
            return;
        }

        const hours = ("0" + this.match.startDate.getHours()).slice(-2);
        const minutes = ("0" + this.match.startDate.getMinutes()).slice(-2);

        this.formattedDate(hours + ":" + minutes);
    }

    private startCountdown(): void {
        this.formattedDate(this.startsInFormatted);
        this.countdownHandle = setInterval(() => this.formattedDate(this.startsInFormatted), 1000);
    }
}