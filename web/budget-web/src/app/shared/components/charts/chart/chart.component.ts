import { Component, ElementRef, Input, OnInit, ViewChild, ViewChildren } from '@angular/core';
import { Chart, registerables } from 'chart.js';
import 'chartjs-adapter-date-fns';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.scss'],
})
export class ChartComponent implements OnInit {
  @Input() type: any;
  @Input() options: any;
  @Input() data: any;
  @Input() plugins: any;

  @ViewChild('canvas') canvas: ElementRef<HTMLCanvasElement>;

  constructor() {}

  ngOnInit(): void {
    console.log('ngOnInit');
  }

  ngAfterViewInit(): void {
    console.log('ngAfterViewInit');

    const context = this.canvas.nativeElement.getContext('2d');
    if (context !== null) {
      Chart.register(...registerables);
      const myChart = new Chart(context, {
        type: this.type,
        data: this.data,
        options: this.options,
        plugins: this.plugins,
      });
    }
  }
}
