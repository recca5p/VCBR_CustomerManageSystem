import { VoTanPhatVCBRDemoTemplatePage } from './app.po';

describe('VoTanPhatVCBRDemo App', function() {
  let page: VoTanPhatVCBRDemoTemplatePage;

  beforeEach(() => {
    page = new VoTanPhatVCBRDemoTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
