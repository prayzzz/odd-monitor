import * as ko from "knockout";
import "./prototype/string"
import { VpGame } from "./odds/vpGame"
import Match from "./models/match"
import { Category } from "./enums/category"

interface CategoryFilter { [c: string]: boolean }

// class CategoryFilter {
//     category: string;
//     categoryName: string;
//     enabled: boolean

//     constructor(category: string, categoryName: string, enabled: boolean) {
//         this.category = category;
//         this.categoryName = categoryName;
//         this.enabled = enabled;
//     }
// }

class MainViewModel {
    private readonly vpGame = new VpGame();
    private matches: KnockoutObservableArray<Match>;
    private filteredMatches: KnockoutObservableArray<Match>;
    private time: KnockoutObservable<string>;
    private filter: CategoryFilter;

    constructor() {
        this.matches = ko.observableArray<Match>();
        this.filteredMatches = ko.observableArray<Match>();
        this.time = ko.observable<string>("");
        this.filter = {};

        this.initFilter();
        this.loadMatches();

        setInterval(() => {
            const date = new Date();
            const hours = ("0" + date.getHours()).slice(-2);
            const minutes = ("0" + date.getMinutes()).slice(-2);
            const seconds = ("0" + date.getSeconds()).slice(-2);

            this.time(hours + ":" + minutes + ":" + seconds);
        }, 1000)
    }

    public filterChanged(parent: MainViewModel) {
        localStorage.setItem("filter", JSON.stringify(parent.filter));

        parent.filteredMatches.removeAll();
        parent.matches().forEach(m => {
            if (parent.filter[Category[m.category]]) {
                parent.filteredMatches.push(m);
            }
        })
    }

    public getImagePath(category: Category): string {
        const name = Category[category].toLowerCase();
        return `/img/category/${name}_64.png`;
    }

    private loadMatches(): void {
        this.vpGame.getMatchesAsync().then(matches => {
            this.matches.removeAll();

            matches.forEach(m => {
                this.matches.push(m);
            })

            this.filterChanged(this);
        })
    }

    private initFilter(): void {
        for (const c in Category) {
            if (c === Category[Category.None]) {
                continue;
            }

            if (typeof Category[c] === 'number') {
                this.filter[c] = true;
            }
        }

        const storedFilterStr = localStorage.getItem("filter");
        if (storedFilterStr) {
            const storedFilter = JSON.parse(storedFilterStr);

            for (var s in storedFilter) {
                if (this.filter.hasOwnProperty(s)) {
                    this.filter[s] = storedFilter[s];
                }
            }
        }
    }
}


ko.applyBindings(new MainViewModel());