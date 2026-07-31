import { Body1, Caption1, Title2, makeStyles, tokens } from '@fluentui/react-components';
import { useQuery } from '@tanstack/react-query';

import { categoriesQuery } from '../api/catalog';
import { AppLink } from '../components/AppLink';
import { AsyncBoundary } from '../components/AsyncBoundary';

const useStyles = makeStyles({
  intro: {
    display: 'flex',
    flexDirection: 'column',
    rowGap: tokens.spacingVerticalXS,
    marginBottom: tokens.spacingVerticalXL,
  },
  list: {
    display: 'flex',
    flexDirection: 'column',
    rowGap: tokens.spacingVerticalL,
    listStyleType: 'none',
    margin: '0',
    padding: '0',
  },
  item: {
    display: 'flex',
    flexDirection: 'column',
    rowGap: tokens.spacingVerticalXXS,
    fontSize: tokens.fontSizeBase500,
  },
});

export function HomePage() {
  const styles = useStyles();
  const { data, isPending, error, refetch } = useQuery(categoriesQuery());

  return (
    <section>
      <div className={styles.intro}>
        <Title2>Browse by category</Title2>
        <Body1>Choose a category to see the books it contains.</Body1>
      </div>

      <AsyncBoundary isPending={isPending} error={error} onRetry={() => void refetch()}>
        <ul className={styles.list}>
          {data?.map((category) => (
            <li key={category.id} className={styles.item}>
              <AppLink to={`/categories/${category.id}`}>{category.name}</AppLink>
              <Caption1>
                {category.bookCount} {category.bookCount === 1 ? 'book' : 'books'}
              </Caption1>
            </li>
          ))}
        </ul>
      </AsyncBoundary>
    </section>
  );
}
