import Ajax from "../shared/ajax"
import { VpGameLogoUrlBuilder as LogoUrl, VpGameCategory } from "./helper"
import Match from "../models/match"
import Team from "../models/team"
import { Wager, WagerTeam } from "../models/wager"
import Tournament from "../models/tournament"

export class VpGame {
    public async getMatchesAsync(): Promise<Array<Match>> {
        const url = "/api/v1/vpgame/match";

        return new Promise<Match[]>((resolve, reject) => {
            Ajax.get<VpGameMatch[]>(url)
                .success(data => {
                    if (data === null) {
                        return;
                    }

                    const matches = data.map(m => this.mapMatch(m));
                    resolve(matches);
                })
                .send();
        });
    }

    public async getWagersAsync(scheduleId: number): Promise<Array<Wager>> {
        const url = "/api/v1/vpgame/wager/" + scheduleId;

        return new Promise<Wager[]>((resolve, reject) => {
            Ajax.get<VpGameWager[]>(url)
                .success(data => {
                    if (data === null) {
                        return;
                    }

                    const wagers = data.map(w => this.mapWager(w));
                    resolve(wagers);
                })
                .send();
        });
    }

    private mapMatch(m: VpGameMatch): Match {
        const tournament = new Tournament(m.tournament.name);

        const team1 = new Team(m.team.left.id, m.team.left.name);
        team1.logoUrl = LogoUrl.build(m.team.left.logo);

        const team2 = new Team(m.team.right.id, m.team.right.name);
        team2.logoUrl = LogoUrl.build(m.team.right.logo);

        const category = VpGameCategory.getCategory(m.category);

        const match = new Match(m.id, new Date(1000 * Number(m.game_time)), team1, team2, category, tournament);
        match.matchLink = `http://dota2.vpgame.com/match/${m.id}.html`;
        match.scheduleId = m.tournament_schedule_id

        return match;
    }

    private mapWager(m: VpGameWager): Wager {
        const team1 = new WagerTeam(new Team(1, "11"), m.odd.left.item);
        const team2 = new WagerTeam(new Team(1, "11"), m.odd.left.item);

        const wager = new Wager(m.id, new Date(1000 * Number(m.game_time)), m.mode_name, team1, team2);
        //wager.status = m.status_name;

        return wager;
    }
}

interface VpGameMatch {
    category: string;
    id: number;
    game_time: number;
    team: VpGameMatchTeams;
    tournament: VpGameTournament;
    tournament_schedule_id: number;
}

interface VpGameWager {
    id: number;
    game_time: number;
    handicap: string;
    handicap_team: string;
    mode_name: string;
    odd: VpGameOdds;
    status_name: string;
}

interface VpGameOdds {
    left: VpGameOdd;
    right: VpGameOdd;
}
interface VpGameOdd {
    item: number;
    victory: string;
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