import Ajax from "../shared/ajax"
import { IMatch, IWager, ITeam } from "./apiModels"
import { Match, Wager, Team } from "../models/models"

export class Loader {
    public static async getMatchesAsync(): Promise<Array<Match>> {
        const url = "/api/v1/vpgame/match";

        return new Promise<Match[]>((resolve, reject) => {
            Ajax.get<IMatch[]>(url)
                .success(data => {
                    if (data !== null) {
                        resolve(data.map(d => this.mapToMatch(d)));
                    }
                    else {
                        reject();
                    }
                })
                .error(data => {
                    reject();
                })
                .send();
        });
    }

    private static mapToMatch(m: IMatch): Match {
        const match = new Match();

        match.additionalData = m.additionalData;
        match.category = m.category;
        match.id = m.id;
        match.matchFormat = m.matchFormat;
        match.matchLink = this.getMatchUrl(m.id);
        match.startDate = new Date(m.startDate);
        match.site = m.site;
        match.teamLeft = this.mapToTeam(m.teamLeft);
        match.teamRight = this.mapToTeam(m.teamRight);
        match.tournamentName = m.tournamentName;
        match.wagers = m.wagers.map(w => this.mapToWager(w));

        return match;
    }

    private static mapToWager(w: IWager): Wager {
        const wager = new Wager();

        wager.additionalData = w.additionalData;
        wager.id = w.id;
        wager.name = w.name;
        wager.oddLeft = w.oddLeft;
        wager.oddRight = w.oddRight;        
        wager.startDate = new Date(w.startDate);        
        wager.status = w.status;
        wager.wagerLink = this.getMatchUrl(w.id);

        return wager;
    }

    private static mapToTeam(t: ITeam): Team {
        const team = new Team();

        team.additionalData = t.additionalData;
        team.id = t.id;
        team.logoUrl = this.getLogoUrl(t.additionalData["logoUrl"])
        team.name = t.name;

        return team;
    }

    public static getLogoUrl(file: string): string {
        return "http://thumb.vpgcdn.com/crop/91x55/" + file;
    }

    public static getMatchUrl(id: number): string {
        return "http://www.vpgame.com/match/" + id + ".html";
    }
}