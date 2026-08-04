import { jsx as _jsx } from "react/jsx-runtime";
import { Link } from '@fluentui/react-components';
import { useHref, useNavigate } from 'react-router';
/**
 * A Fluent UI link that navigates through the router. It renders a real anchor, so "open in new
 * tab" and modified clicks keep working, while a plain click stays inside the SPA.
 */
export function AppLink({ to, children }) {
    const href = useHref(to);
    const navigate = useNavigate();
    return (_jsx(Link, { href: href, onClick: (event) => {
            const isModifiedClick = event.metaKey || event.ctrlKey || event.shiftKey || event.altKey || event.button !== 0;
            if (event.defaultPrevented || isModifiedClick) {
                return;
            }
            event.preventDefault();
            void navigate(to);
        }, children: children }));
}
