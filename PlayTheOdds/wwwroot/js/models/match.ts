import Team from "./team"
import Tournament from "./tournament"
import { Category } from "../enums/category"

export default class Match {
    category: Category;
    date: Date;
    id: number;
    team1: Team;
    team2: Team;
    tournament: Tournament;

    constructor(id: number, date: Date, team1: Team, team2: Team, category: Category, tournament: Tournament) {
        this.category = category;
        this.date = date;
        this.id = id;
        this.team1 = team1;
        this.team2 = team2;
        this.tournament = tournament;
    }

    public get formattedDate(): string {
        const hours = ("0" + this.date.getHours()).slice(-2);
        const minutes = ("0" + this.date.getMinutes()).slice(-2);

        return hours + ":" + minutes
    }
}