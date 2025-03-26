import {
  Component,
  ElementRef,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { Chart, registerables } from 'chart.js';
import 'chartjs-adapter-date-fns';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: [],
})
export class ChartComponent implements OnInit, OnChanges {
  @Input() type: any;
  @Input() options: any;
  @Input() data: any;
  @Input() plugins: any;

  @ViewChild('canvas') canvas: ElementRef<HTMLCanvasElement>;

  chart: any;

  constructor() {}

  ngOnChanges(changes: SimpleChanges): void {
    if (this.chart) {
      this.chart.data = this.data;
      this.chart.options = this.options;
      this.chart.plugins = this.plugins;
      this.chart.update();
    }
  }

  ngOnInit(): void {}

  ngAfterViewInit(): void {
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
