import * as Enums from "./enums";
import { IStringDictionary } from "../shared/types";

export class Match {
    public additionalData: IStringDictionary<string>;
    public category: Enums.Category;
    public id: number;
    public matchFormat: Enums.MatchFormat;
    public matchLink: string;
    public startDate: Date;
    public site: Enums.Site;
    public teamLeft: Team;
    public teamRight: Team;
    public tournamentName: string;
    public wagers: Wager[];
}

export class Team {
    public additionalData: IStringDictionary<string>;
    public id: number;
    public logoUrl: string;
    public name: string;
}

export class Wager {
    public additionalData: IStringDictionary<string>;
    public id: number;
    public name: string;
    public oddLeft: number;
    public oddRight: number;
    public startDate: Date;
    public status: Enums.WagerStatus;
    public wagerLink: string;
}
