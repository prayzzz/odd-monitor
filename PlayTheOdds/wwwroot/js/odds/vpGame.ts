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

            Ajax.get<VpGameEnvelop<VpGameMatch[]>>(url)
                .success(data => {
                    const matches = data.body.map(vpMatch => this.map(vpMatch));
                    resolve(matches);
                })
                .send();
        })
    }

    private map(vpMatch: VpGameMatch): Match {
        const tournament = new Tournament(vpMatch.tournament.name);

        const team1 = new Team(vpMatch.team.left.id, vpMatch.team.left.name);
        team1.logoUrl = new LogoUrl().build(vpMatch.team.left.logo);

        const team2 = new Team(vpMatch.team.right.id, vpMatch.team.right.name);
        team2.logoUrl = new LogoUrl().build(vpMatch.team.right.logo);

        const category = VpGameCategory.getCategory(vpMatch.category);

        return new Match(vpMatch.id, new Date(1000 * Number(vpMatch.game_time)), team1, team2, category, tournament);
    }
}



interface VpGameEnvelop<T> {
    body: T;
    current_time: number;
    message: string;
    status: number;
    success: boolean;
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