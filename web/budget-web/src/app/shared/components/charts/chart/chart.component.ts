import {
  Component,
  ElementRef,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
  ViewChild,
  ViewChildren,
} from '@angular/core';
import { Chart, registerables } from 'chart.js';
import 'chartjs-adapter-date-fns';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.scss'],
})
export class ChartComponent implements OnInit, OnChanges {
  @Input() type: any;
  @Input() options: any;
  @Input() data: any;
  @Input() plugins: any;

  @ViewChild('canvas') canvas: ElementRef<HTMLCanvasElement>;

  chart: Chart;

  constructor() {}

  ngOnChanges(changes: SimpleChanges): void {
    // console.log('changes detected', changes);
    console.log('data', this.data);
    if (this.chart) {
      this.chart.data = this.data;
      this.chart.options = this.options;
      this.chart.update();
    }
  }

  ngOnInit(): void {
    console.log('ngOnInit');
  }

  ngAfterViewInit(): void {
    console.log('ngAfterViewInit');

    const context = this.canvas.nativeElement.getContext('2d');
    if (context !== null) {
      Chart.register(...registerables);
      this.chart = new Chart(context, {
        type: this.type,
        data: this.data,
        options: this.options,
        plugins: this.plugins,
      });
    }
  }
}
