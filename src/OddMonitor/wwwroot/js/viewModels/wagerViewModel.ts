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

    public get wagerLink(): string {
        return this.wager.wagerLink;
    }

    public get status(): string {
        return Enums.WagerStatus[this.wager.status];
    }

    public get rightHandicap(): string {
        if (!this.hasHandicap) {
            return "";
        }

        if (this.hasLeftHandicap) {
            return "[+" + this.wager.additionalData["handicap"] + "]";
        }

        return "[-" + this.wager.additionalData["handicap"] + "]";
    }

    public get leftHandicap(): string {
        if (!this.hasHandicap) {
            return "";
        }

        if (this.hasRightHandicap) {
            return "[+" + this.wager.additionalData["handicap"] + "]";
        }

        return "[-" + this.wager.additionalData["handicap"] + "]";
    }

    public get hasHandicap(): boolean {
        return this.wager.additionalData["handicapTeam"] !== "none";
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
        return !this.isLive && this.startsIn < Timespan.fromMinutes(30);
    }

    public get isLive(): boolean {
        const current = new Date().getTime();
        const wager = this.wager.startDate.getTime();

        return wager < current;
    }

    private initFormattedDate() {
        if (this.isLive) {
            this.formattedStatus("Live");
            return;
        }

        if (this.wager.status === Enums.WagerStatus.Open) {
            const hours = ("0" + this.wager.startDate.getHours()).slice(-2);
            const minutes = ("0" + this.wager.startDate.getMinutes()).slice(-2);

            this.formattedStatus(hours + ":" + minutes);
            return;
        }

        this.formattedStatus(this.status);
    }
}