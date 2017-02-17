import Team from "../models/team"
import { WagerStatus } from "../enums/status"

export class Wager {
    public id: number;
    public date: Date;
    public mode: string;
    public status: WagerStatus;
    public team1: WagerTeam;
    public team2: WagerTeam;

    constructor(id: number, date: Date, mode: string, team1: WagerTeam, team2: WagerTeam) {
    }
}

export class WagerTeam {
    public handicap: string | null;
    public odd: number;
    public team: Team;

    constructor(team: Team, odd: number) {
    }
}