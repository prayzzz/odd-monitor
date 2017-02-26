import * as Enums from "./enums"
import { IStringDictionary } from "../shared/types"

export class Match {
    additionalData: IStringDictionary<string>;
    category: Enums.Category;
    id: number;
    matchFormat: Enums.MatchFormat;
    matchLink: string;
    startDate: Date;
    site: Enums.Site;
    teamLeft: Team;
    teamRight: Team;
    tournamentName: string;
    wagers: Wager[];
}

export class Team {
    additionalData: IStringDictionary<string>;
    id: number;
    logoUrl: string;
    name: string;
}

export class Wager {
    additionalData: IStringDictionary<string>;
    id: number;
    name: string;
    oddLeft: number;
    oddRight: number;
    startDate: Date;
    status: Enums.WagerStatus;
    wagerLink: string;
}