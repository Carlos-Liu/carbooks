import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { Caption1, Title1, makeStyles, tokens } from '@fluentui/react-components';
import { Route, Routes } from 'react-router';
import { AddBookPage } from './pages/AddBookPage';
import { CategoryBooksPage } from './pages/CategoryBooksPage';
import { HomePage } from './pages/HomePage';
import { NotFoundPage } from './pages/NotFoundPage';
const useStyles = makeStyles({
    shell: {
        minHeight: '100%',
        display: 'flex',
        flexDirection: 'column',
    },
    header: {
        padding: `${tokens.spacingVerticalXL} ${tokens.spacingHorizontalXXL}`,
        backgroundColor: tokens.colorNeutralBackground1,
        borderBottom: `1px solid ${tokens.colorNeutralStroke2}`,
    },
    main: {
        flexGrow: 1,
        width: '100%',
        maxWidth: '1080px',
        margin: '0 auto',
        padding: `${tokens.spacingVerticalXXL} ${tokens.spacingHorizontalXXL}`,
    },
});
export default function App() {
    const styles = useStyles();
    return (_jsxs("div", { className: styles.shell, children: [_jsxs("header", { className: styles.header, children: [_jsx(Title1, { children: "CarBooks" }), _jsx(Caption1, { children: " A small catalog of motoring books." })] }), _jsx("main", { className: styles.main, children: _jsxs(Routes, { children: [_jsx(Route, { path: "/", element: _jsx(HomePage, {}) }), _jsx(Route, { path: "/books/new", element: _jsx(AddBookPage, {}) }), _jsx(Route, { path: "/categories/:categoryId", element: _jsx(CategoryBooksPage, {}) }), _jsx(Route, { path: "*", element: _jsx(NotFoundPage, {}) })] }) })] }));
}
