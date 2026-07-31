import { Body1, Title2, makeStyles, tokens } from '@fluentui/react-components';
import { useQuery } from '@tanstack/react-query';
import { useParams } from 'react-router';

import { categoryBooksQuery } from '../api/catalog';
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

export function CategoryBooksPage() {
  const styles = useStyles();
  const { slug = '' } = useParams<{ slug: string }>();
  const { data, isPending, error, refetch } = useQuery(categoryBooksQuery(slug));

  return (
    <section>
      <div className={styles.header}>
        <AppLink to="/">Back to categories</AppLink>
        <Title2>{data?.category.name ?? 'Books'}</Title2>
        <Body1>Every book in this category, served by the CarBooks API.</Body1>
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
