import * as ko from "knockout";

import { VpGame } from "../odds/vpGame"
import Match from "../models/match"
import Timespan from "../shared/timespan"
import Tournament from "../models/tournament";
import Team from "../models/team";
import Wager from "../models/wager";
import { Category } from "../enums/category";

export default class MatchViewModel {
    private readonly vpGame = new VpGame();
    private countdownHandle: number;

    public readonly formattedDate: KnockoutObservable<string>;
    public readonly match: Match;
    public readonly wagers: KnockoutObservableArray<Wager>;

    constructor(match: Match) {
        this.match = match;
        this.formattedDate = ko.observable<string>();
        this.wagers = ko.observableArray<Wager>();

        this.initFormattedDate();

        this.vpGame.getWagersAsync(this.match.scheduleId)
            .then(wagers => {
                this.wagers.removeAll();

                wagers.forEach(w => this.wagers.push(w));
            })
    }

    public get tournament(): Tournament {
        return this.match.tournament;
    }

    public get category(): Category {
        return this.match.category;
    }

    public get team1(): Team {
        return this.match.team1;
    }

    public get team2(): Team {
        return this.match.team2;
    }

    public get matchLink(): string {
        return this.match.matchLink;
    }

    /**
     * Time to start in milliseconds
     */
    public get startsIn(): number {
        return this.match.date.getTime() - new Date().getTime();
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
        const match = this.match.date.getTime();

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

        const hours = ("0" + this.match.date.getHours()).slice(-2);
        const minutes = ("0" + this.match.date.getMinutes()).slice(-2);

        this.formattedDate(hours + ":" + minutes);
    }

    private startCountdown(): void {
        this.formattedDate(this.startsInFormatted);
        this.countdownHandle = setInterval(() => this.formattedDate(this.startsInFormatted), 1000);
    }
}