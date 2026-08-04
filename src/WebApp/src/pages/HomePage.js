import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { Body1, Caption1, Title2, makeStyles, tokens } from '@fluentui/react-components';
import { useQuery } from '@tanstack/react-query';
import { categoriesQuery } from '../api/catalog';
import { AppLink } from '../components/AppLink';
import { AsyncBoundary } from '../components/AsyncBoundary';
const useStyles = makeStyles({
    intro: {
        display: 'flex',
        flexDirection: 'column',
        rowGap: tokens.spacingVerticalXS,
        marginBottom: tokens.spacingVerticalXL,
    },
    introRow: {
        display: 'flex',
        flexWrap: 'wrap',
        justifyContent: 'space-between',
        alignItems: 'flex-start',
        columnGap: tokens.spacingHorizontalL,
        rowGap: tokens.spacingVerticalS,
    },
    list: {
        display: 'flex',
        flexDirection: 'column',
        rowGap: tokens.spacingVerticalL,
        listStyleType: 'none',
        margin: '0',
        padding: '0',
    },
    item: {
        display: 'flex',
        flexDirection: 'column',
        rowGap: tokens.spacingVerticalXXS,
        fontSize: tokens.fontSizeBase500,
    },
});
export function HomePage() {
    const styles = useStyles();
    const { data, isPending, error, refetch } = useQuery(categoriesQuery());
    return (_jsxs("section", { children: [_jsx("div", { className: styles.intro, children: _jsxs("div", { className: styles.introRow, children: [_jsxs("div", { children: [_jsx(Title2, { children: "Browse by category" }), _jsx(Body1, { children: "Choose a category to see the books it contains." })] }), _jsx(AppLink, { to: "/books/new", children: "Add a book" })] }) }), _jsx(AsyncBoundary, { isPending: isPending, error: error, onRetry: () => void refetch(), children: _jsx("ul", { className: styles.list, children: data?.map((category) => (_jsxs("li", { className: styles.item, children: [_jsx(AppLink, { to: `/categories/${category.id}`, children: category.name }), _jsxs(Caption1, { children: [category.bookCount, " ", category.bookCount === 1 ? 'book' : 'books'] })] }, category.id))) }) })] }));
}
