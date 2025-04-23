import { HpxmlGenerationService } from './../../shared/services/hpxml-generation/hpxml-generation.service';
import { Component, Input, OnInit } from '@angular/core';
import { Unsubscriber } from '../../shared/modules/unsubscribe/unsubscribe.component.';
import { takeUntil } from 'rxjs';
import { Result } from '../../shared/models/common/result.model';

@Component({
  selector: 'app-print-hpxml',
  standalone: false,
  templateUrl: './print-hpxml.component.html',
  styleUrl: './print-hpxml.component.scss'
})
export class PrintHpxmlComponent extends Unsubscriber implements OnInit {

  hpxmlString: string | null | undefined;

  @Input("buildingId") buildingId: string | null | undefined

  constructor(private  hpxmlGenerationService:HpxmlGenerationService) {
    super();
  }

  ngOnInit(): void {

  }

  formatXml() {
    const PADDING = '  ';
    const stack = [];
    let formatted = '';
    let indent = 0;

    let xml = this.hpxmlString?.replace(/>\s*</g, '><').trim();
    if (xml == null) return;

    const tokens = xml.split(/(<[^>]+>)/).filter(Boolean);

    for (let i = 0; i < tokens.length; i++) {
      const token = tokens[i];
      const next = tokens[i + 1];
      const nextNext = tokens[i + 2];

      // Collapse <tag>text</tag>
      if (
        token.match(/^<[^\/!?][^>]*>$/) &&         // opening tag
        next && !next.startsWith('<') &&           // text content
        nextNext && nextNext.match(/^<\/[^>]+>$/)  // closing tag
      ) {
        formatted += PADDING.repeat(indent) + token + next.trim() + nextNext + '\n';
        i += 2;
        continue;
      }

      if (token.match(/^<\?.*\?>$/) || token.match(/^<!--.*-->$/)) {
        formatted += PADDING.repeat(indent) + token + '\n';
      } else if (token.match(/^<\/.+>$/)) {
        indent = Math.max(indent - 1, 0);
        formatted += PADDING.repeat(indent) + token + '\n';
        stack.pop();
      } else if (token.match(/^<[^\/!?][^>]*[^\/]>$/)) {
        formatted += PADDING.repeat(indent) + token + '\n';
        stack.push(token);
        indent++;
      } else if (token.match(/^<[^>]+\/>$/)) {
        formatted += PADDING.repeat(indent) + token + '\n';
      } else {
        formatted += PADDING.repeat(indent) + token.trim() + '\n';
      }
    }

    // Escape for HTML rendering
    this.hpxmlString = formatted
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;');
  }


  generateHPXML() {
    if(this.buildingId){
      this.hpxmlGenerationService.getHpxmlString(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
        next:(res:Result<string>)=>{
          if(res.failed == false){
            this.hpxmlString = res?.data;
            this.formatXml();
          }
        },
        error:err=>console.log(err)
      })
    }
  }
}
