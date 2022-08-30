import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

@Component({
	selector: 'ec-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
	public title = 'EventCatalog';
	public forecasts?: WeatherForecast[];
	public events$: Observable<Event[]> = this.http.get<Event[]>('/api/events');

	constructor(private http: HttpClient) {}

	ngOnInit(): void {
		this.http.get<WeatherForecast[]>('/weatherforecast')
			.subscribe(result => {
				this.forecasts = result;
			}, error => console.error(error));
	}
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

interface Event {
	id: number;
	name: string;
	type: string;
	city: string;
	street: string;
	venue: string;
	startTime: string;
	endTime: string;
	organizerSite: string;
	price: number;
	currency: number;
	description: string;
	imageFileName: string;
	potentialAttendees: object[];
}
