import { IStringDictionary } from "../shared/types"

export interface IMatch {
    additionalData: IStringDictionary<string>;
    category: number;
    id: number;
    matchFormat: number;
    matchLink: string;
    startDate: string;
    site: number;
    teamLeft: ITeam;
    teamRight: ITeam;
    tournamentName: string;
    wagers: IWager[];
}

export interface ITeam {
    additionalData: IStringDictionary<string>;
    id: number;
    name: string;
}

export interface IWager {
    additionalData: IStringDictionary<string>;
    id: number;
    name: string;
    oddLeft: number;
    oddRight: number;
    startDate: string;
    status: number;
}