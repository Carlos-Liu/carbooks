import type { ReactNode } from 'react';
interface AsyncBoundaryProps {
    isPending: boolean;
    error: unknown;
    onRetry: () => void;
    children: ReactNode;
}
/** Renders the loading and failure states of a query so pages only describe the happy path. */
export declare function AsyncBoundary({ isPending, error, onRetry, children }: AsyncBoundaryProps): import("react").JSX.Element;
export {};
