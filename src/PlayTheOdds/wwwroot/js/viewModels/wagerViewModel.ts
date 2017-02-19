import * as ko from "knockout";

import Timespan from "../shared/timespan"
import * as Enums from "../models/enums";
import { Team, Wager } from "../models/models"

export default class WagerViewModel {
    public wager: Wager;
    private teamLeft: Team;
    private teamRight: Team;
    public readonly formattedStatus: KnockoutObservable<string>;

    constructor(wager: Wager, teamLeft: Team, teamRight: Team) {
        this.wager = wager;
        this.teamLeft = teamLeft;
        this.teamRight = teamRight;

        this.formattedStatus = ko.observable<string>();
        this.initFormattedDate();
    }

    public get oddLeft(): string {
        return this.wager.oddLeft.toFixed(2);
    }

    public get oddRight(): string {
        return this.wager.oddRight.toFixed(2);
    }

    public get name(): string {
        return this.wager.name;
    }

    public get status(): string {
        return Enums.WagerStatus[this.wager.status];
    }

    public get handicap(): string {
        if (!this.hasHandicap) {
            return "";
        }

        return "[-" + this.wager.additionalData["handicap"] + "]";
    }

    public get nameWithHandicap(): string {
        if (!this.hasHandicap) {
            return this.wager.name;
        }

        let handicap = this.wager.additionalData["handicap"];
        let teamName = this.teamLeft.name;

        if (this.wager.additionalData["handicapTeam"] === "right") {
            teamName = this.teamRight.name;
        }

        if (teamName.length > 10) {
            teamName = teamName.slice(0, 15).trim() + "...";
        }

        return `${this.wager.name} [${teamName} -${handicap}]`;
    }

    public get hasHandicap(): boolean {
        return this.wager.additionalData["handicapTeam"] != "none";
    }

    public get hasLeftHandicap(): boolean {
        return this.wager.additionalData["handicapTeam"] === "left";
    }

    public get hasRightHandicap(): boolean {
        return this.wager.additionalData["handicapTeam"] === "right";
    }

    public get startsIn(): number {
        return this.wager.startDate.getTime() - new Date().getTime();
    }
    public get isStartingSoon(): boolean {
        return this.startsIn < Timespan.fromMinutes(30);
    }

    public get isLive(): boolean {
        const current = new Date().getTime();
        const match = this.wager.startDate.getTime();

        return match < current;
    }

    private initFormattedDate() {
        if (this.wager.status === Enums.WagerStatus.Open) {
            const hours = ("0" + this.wager.startDate.getHours()).slice(-2);
            const minutes = ("0" + this.wager.startDate.getMinutes()).slice(-2);

            this.formattedStatus(hours + ":" + minutes);
            return;
        }

        this.formattedStatus(this.status);
    }
}