import { jsx as _jsx, jsxs as _jsxs, Fragment as _Fragment } from "react/jsx-runtime";
import { Button, MessageBar, MessageBarActions, MessageBarBody, MessageBarTitle, Spinner } from '@fluentui/react-components';
/** Renders the loading and failure states of a query so pages only describe the happy path. */
export function AsyncBoundary({ isPending, error, onRetry, children }) {
    if (isPending) {
        return _jsx(Spinner, { labelPosition: "below", label: "Loading the catalog\u2026" });
    }
    if (error) {
        return (_jsxs(MessageBar, { intent: "error", children: [_jsxs(MessageBarBody, { children: [_jsx(MessageBarTitle, { children: "Could not load data" }), error instanceof Error ? error.message : 'An unexpected error occurred.'] }), _jsx(MessageBarActions, { children: _jsx(Button, { onClick: onRetry, children: "Try again" }) })] }));
    }
    return _jsx(_Fragment, { children: children });
}
