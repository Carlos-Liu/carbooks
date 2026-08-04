import type { ReactNode } from 'react';
interface AppLinkProps {
    to: string;
    children: ReactNode;
}
/**
 * A Fluent UI link that navigates through the router. It renders a real anchor, so "open in new
 * tab" and modified clicks keep working, while a plain click stays inside the SPA.
 */
export declare function AppLink({ to, children }: AppLinkProps): import("react").JSX.Element;
export {};
