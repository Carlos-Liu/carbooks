import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { Body1, Title2, makeStyles, tokens } from '@fluentui/react-components';
import { useQuery } from '@tanstack/react-query';
import { useParams } from 'react-router';
import { categoryBooksQuery } from '../api/catalog';
import { AppLink } from '../components/AppLink';
import { AsyncBoundary } from '../components/AsyncBoundary';
import { BookCard } from '../components/BookCard';
const useStyles = makeStyles({
    header: {
        display: 'flex',
        flexDirection: 'column',
        rowGap: tokens.spacingVerticalXS,
        marginBottom: tokens.spacingVerticalXL,
    },
    grid: {
        display: 'flex',
        flexWrap: 'wrap',
        gap: tokens.spacingHorizontalL,
    },
});
export function CategoryBooksPage() {
    const styles = useStyles();
    const { categoryId = '' } = useParams();
    const { data, isPending, error, refetch } = useQuery(categoryBooksQuery(categoryId));
    return (_jsxs("section", { children: [_jsxs("div", { className: styles.header, children: [_jsx(AppLink, { to: "/", children: "Back to categories" }), _jsx(Title2, { children: data?.category.name ?? 'Books' }), _jsx(Body1, { children: "Every book in this category, served by the CarBooks API." })] }), _jsx(AsyncBoundary, { isPending: isPending, error: error, onRetry: () => void refetch(), children: _jsx("div", { className: styles.grid, children: data?.books.map((book) => (_jsx(BookCard, { book: book }, book.id))) }) })] }));
}
