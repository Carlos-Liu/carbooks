import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { Body1, Title2, makeStyles, tokens } from '@fluentui/react-components';
import { AppLink } from '../components/AppLink';
const useStyles = makeStyles({
    root: {
        display: 'flex',
        flexDirection: 'column',
        rowGap: tokens.spacingVerticalM,
        alignItems: 'flex-start',
    },
});
export function NotFoundPage() {
    const styles = useStyles();
    return (_jsxs("section", { className: styles.root, children: [_jsx(Title2, { children: "Page not found" }), _jsx(Body1, { children: "The page you asked for does not exist." }), _jsx(AppLink, { to: "/", children: "Back to categories" })] }));
}
