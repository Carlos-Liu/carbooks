import { Body1, Caption1, Title2, makeStyles, tokens } from '@fluentui/react-components';
import { useQuery } from '@tanstack/react-query';

import { tagsQuery } from '../api/catalog';
import { AppLink } from '../components/AppLink';
import { AsyncBoundary } from '../components/AsyncBoundary';
import { TagChip } from '../components/TagChip';

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
  nav: {
    display: 'flex',
    flexWrap: 'wrap',
    columnGap: tokens.spacingHorizontalL,
    rowGap: tokens.spacingVerticalXS,
  },
  list: {
    display: 'flex',
    flexWrap: 'wrap',
    gap: tokens.spacingHorizontalS,
    listStyleType: 'none',
    margin: '0',
    padding: '0',
  },
  empty: {
    color: tokens.colorNeutralForeground3,
  },
});

export function TagsPage() {
  const styles = useStyles();
  const { data, isPending, error, refetch } = useQuery(tagsQuery());

  return (
    <section>
      <div className={styles.intro}>
        <div className={styles.introRow}>
          <div>
            <Title2>All tags</Title2>
            <Body1>Labels that can be shared across books in the catalog.</Body1>
          </div>
          <div className={styles.nav}>
            <AppLink to="/">Categories</AppLink>
            <AppLink to="/books/new">Add a book</AppLink>
          </div>
        </div>
      </div>

      <AsyncBoundary isPending={isPending} error={error} onRetry={() => void refetch()}>
        {data && data.length > 0 ? (
          <ul className={styles.list} aria-label="All tags">
            {data.map((tag) => (
              <li key={tag.id}>
                <TagChip name={tag.name} id={tag.id} />
              </li>
            ))}
          </ul>
        ) : (
          <Caption1 className={styles.empty}>No tags yet.</Caption1>
        )}
      </AsyncBoundary>
    </section>
  );
}
