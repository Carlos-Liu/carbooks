import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { Caption1, Card, CardHeader, CardPreview, Link, Text, makeStyles, shorthands, tokens, } from '@fluentui/react-components';
const useStyles = makeStyles({
    card: {
        width: '260px',
    },
    preview: {
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        minHeight: '260px',
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
    // Prefer the locally stored cover when the publisher host is unreachable.
    const coverSource = book.coverImage ?? book.coverUrl ?? undefined;
    return (_jsxs(Card, { className: styles.card, children: [_jsx(CardPreview, { className: styles.preview, children: coverSource ? (_jsx("img", { className: styles.cover, src: coverSource, alt: `Cover of ${book.name}`, loading: "lazy" })) : (_jsx(Caption1, { children: "No cover" })) }), _jsx(CardHeader, { header: _jsx(Text, { weight: "semibold", children: book.name }), description: _jsxs(Caption1, { children: [book.author, book.translator ? ` · tr. ${book.translator}` : '', book.publisher ? ` · ${book.publisher}` : '', book.publishedOn ? ` · ${book.publishedOn}` : '', book.isbn ? ` · ISBN ${book.isbn}` : ''] }) }), book.recommendation ? (_jsx("div", { className: styles.footer, children: _jsx(Caption1, { children: book.recommendation }) })) : null, book.coverUrl ? (_jsx("div", { className: styles.footer, children: _jsx(Link, { href: book.coverUrl, target: "_blank", rel: "noreferrer", children: "Original cover" }) })) : null] }));
}
