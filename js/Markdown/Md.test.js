import renderToHtml from './Md'

describe('renderToHtml', () => {
    test('Должен возвращать строку', () => {
        expect(typeof renderToHtml('dds')).toBe('string');
    });
})


