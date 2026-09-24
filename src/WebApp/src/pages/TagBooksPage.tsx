import { Body1, Title2, makeStyles, tokens } from '@fluentui/react-components';
import { useQuery } from '@tanstack/react-query';
import { useParams } from 'react-router';

import { tagBooksQuery } from '../api/catalog';
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

export function TagBooksPage() {
  const styles = useStyles();
  const { tagId = '' } = useParams<{ tagId: string }>();
  const { data, isPending, error, refetch } = useQuery(tagBooksQuery(tagId));

  return (
    <section>
      <div className={styles.header}>
        <AppLink to="/tags">Back to tags</AppLink>
        <Title2>{data?.tag.name ?? 'Books'}</Title2>
        <Body1>Every book labeled with this tag, served by the CarBooks API.</Body1>
      </div>

      <AsyncBoundary isPending={isPending} error={error} onRetry={() => void refetch()}>
        <div className={styles.grid}>
          {data?.books.map((book) => (
            <BookCard key={book.id} book={book} />
          ))}
        </div>
      </AsyncBoundary>
    </section>
  );
}
