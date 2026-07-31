import { Link } from '@fluentui/react-components';
import type { ReactNode } from 'react';
import { useHref, useNavigate } from 'react-router';

interface AppLinkProps {
  to: string;
  children: ReactNode;
}

/**
 * A Fluent UI link that navigates through the router. It renders a real anchor, so "open in new
 * tab" and modified clicks keep working, while a plain click stays inside the SPA.
 */
export function AppLink({ to, children }: AppLinkProps) {
  const href = useHref(to);
  const navigate = useNavigate();

  return (
    <Link
      href={href}
      onClick={(event) => {
        const isModifiedClick =
          event.metaKey || event.ctrlKey || event.shiftKey || event.altKey || event.button !== 0;

        if (event.defaultPrevented || isModifiedClick) {
          return;
        }

        event.preventDefault();
        void navigate(to);
      }}
    >
      {children}
    </Link>
  );
}
