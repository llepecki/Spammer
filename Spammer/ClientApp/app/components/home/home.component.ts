import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})

export class HomeComponent {
    public topics: Topic[];
    public subscription: Subscription;
    public mail: string;
    public loaded: boolean;
    private http: Http;
    private baseUrl: string;

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this.mail = '';
        this.loaded = false;
        this.http = http;
        this.baseUrl = baseUrl;

        http.get(baseUrl + 'api/topics').subscribe(result => {
            this.topics = result.json() as Topic[];
        }, error => console.error(error));
    }

    public load() {
        if (this.mail !== '') {
            this.http.get(this.baseUrl + 'api/subscriptions/' + this.mail).subscribe(result => {
                for (let i = 0; i < this.topics.length; i++) {
                    this.topics[i].selected = false;
                }

                this.subscription = result.json() as Subscription;

                for (let i = 0; i < this.subscription.topics.length; i++) {
                    for (let j = 0; j < this.topics.length; j++) {
                        if (this.topics[j].abbreviation === this.subscription.topics[i].abbreviation) {
                            this.topics[j].selected = true;
                        }
                    }
                }

                this.loaded = true;
            }, error => {
                console.error(error);
                window.alert(error);
            });
        }
    }

    public submit() {
        if (this.mail !== '') {
            let selected = this.getSelectedTopicAbbreviations();

            this.http.put(this.baseUrl + 'api/subscriptions/' + this.mail, selected).subscribe(result => {
                console.debug(result);
                window.alert(result);
                this.reset();
            }, error => {
                console.error(error);
                window.alert(error);
            });
        }
    }

    public unsubscribe() {
        if (this.mail !== '') {
            this.http.delete(this.baseUrl + 'api/subscriptions/' + this.mail).subscribe(result => {
                console.debug(result);
                window.alert(result)
                this.reset();
            }, error => {
                console.error(error);
                window.alert(error);
            });        }
    }

    private getSelectedTopicAbbreviations() {
        let selected = [];

        for (let i = 0; i < this.topics.length; i++) {
            if (this.topics[i].selected) {
                selected.push(this.topics[i].abbreviation);
            }
        }

        return selected;
    }

    private reset() {
        this.mail = '';
        this.loaded = false;

        for (let i = 0; i < this.topics.length; i++) {
            this.topics[i].selected = false;
        }
    }
}

interface Topic {
    abbreviation: string;
    description: string;
    selected: boolean;
}

interface Subscription {
    mail: string;
    topics: Topic[];
}