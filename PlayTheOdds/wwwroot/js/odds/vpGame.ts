import Ajax from "../shared/ajax"
import { VpGameMatchUrlBuilder as MatchUrl, VpGameLogoUrlBuilder as LogoUrl, VpGameCategory } from "./helper"
import Match from "../models/match"
import Team from "../models/team"
import Tournament from "../models/tournament"
import { Status } from "../enums/status"
import { Category } from "../enums/category"

export class VpGame {
    public async getMatchesAsync(): Promise<Array<Match>> {
        const url = new MatchUrl().withStatus(Status.Open)
            .withCategory(Category.None)
            .build();

        return new Promise<Match[]>((resolve, reject) => {

            Ajax.get<VpGameMatch[]>(url)
                .success(data => {
                    if (data === null) {
                        return;
                    }

                    const matches = data.map(m => this.map(m));
                    resolve(matches);
                })
                .send();
        });
    }

    private map(m: VpGameMatch): Match {
        const tournament = new Tournament(m.tournament.name);

        const team1 = new Team(m.team.left.id, m.team.left.name);
        team1.logoUrl = new LogoUrl().build(m.team.left.logo);

        const team2 = new Team(m.team.right.id, m.team.right.name);
        team2.logoUrl = new LogoUrl().build(m.team.right.logo);

        const category = VpGameCategory.getCategory(m.category);

        const match = new Match(m.id, new Date(1000 * Number(m.game_time)), team1, team2, category, tournament);
        match.matchLinkBuilder = this.buildMatchLink;

        return match;
    }

    private buildMatchLink(m: Match): string {
        return `http://dota2.vpgame.com/match/${m.id}.html`;
    }
}


interface VpGameMatch {
    category: string;
    id: number;
    game_time: number;
    team: VpGameMatchTeams;
    tournament: VpGameTournament;
}

interface VpGameMatchTeams {
    left: VpGameTeam;
    right: VpGameTeam;
}

interface VpGameTeam {
    id: number;
    logo: string;
    name: string;
}

interface VpGameTournament {
    category: string;
    logo: string;
    name: string;
}