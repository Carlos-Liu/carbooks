import { Button, MessageBar, MessageBarActions, MessageBarBody, MessageBarTitle, Spinner } from '@fluentui/react-components';
import type { ReactNode } from 'react';

interface AsyncBoundaryProps {
  isPending: boolean;
  error: unknown;
  onRetry: () => void;
  children: ReactNode;
}

/** Renders the loading and failure states of a query so pages only describe the happy path. */
export function AsyncBoundary({ isPending, error, onRetry, children }: AsyncBoundaryProps) {
  if (isPending) {
    return <Spinner labelPosition="below" label="Loading the catalog…" />;
  }

  if (error) {
    return (
      <MessageBar intent="error">
        <MessageBarBody>
          <MessageBarTitle>Could not load data</MessageBarTitle>
          {error instanceof Error ? error.message : 'An unexpected error occurred.'}
        </MessageBarBody>
        <MessageBarActions>
          <Button onClick={onRetry}>Try again</Button>
        </MessageBarActions>
      </MessageBar>
    );
  }

  return <>{children}</>;
}
