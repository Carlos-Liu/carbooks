import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { Caption1, Card, CardHeader, CardPreview, Link, Text, makeStyles, shorthands, tokens, } from '@fluentui/react-components';
const useStyles = makeStyles({
    card: {
        width: '260px',
    },
    preview: {
        display: 'flex',
        justifyContent: 'center',
        backgroundColor: tokens.colorNeutralBackground3,
    },
    cover: {
        height: '260px',
        width: 'auto',
        objectFit: 'contain',
    },
    footer: {
        ...shorthands.padding('0', tokens.spacingHorizontalM, tokens.spacingVerticalM),
    },
});
export function BookCard({ book }) {
    const styles = useStyles();
    // The locally stored cover keeps the page intact when the publisher host is unreachable.
    const coverSource = book.coverImage ?? book.coverUrl;
    return (_jsxs(Card, { className: styles.card, children: [_jsx(CardPreview, { className: styles.preview, children: _jsx("img", { className: styles.cover, src: coverSource, alt: `Cover of ${book.name}`, loading: "lazy" }) }), _jsx(CardHeader, { header: _jsx(Text, { weight: "semibold", children: book.name }), description: _jsx(Caption1, { children: book.author }) }), _jsx("div", { className: styles.footer, children: _jsx(Link, { href: book.coverUrl, target: "_blank", rel: "noreferrer", children: "Original cover" }) })] }));
}
