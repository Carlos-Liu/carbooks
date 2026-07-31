import {
  Caption1,
  Card,
  CardHeader,
  CardPreview,
  Link,
  Text,
  makeStyles,
  shorthands,
  tokens,
} from '@fluentui/react-components';

import type { Book } from '../api/types';

const useStyles = makeStyles({
  card: {
    width: '260px',
  },
  preview: {
    display: 'flex',
    justifyContent: 'center',
    backgroundColor: tokens.colorNeutralBackground3,
  },
  cover: {
    height: '260px',
    width: 'auto',
    objectFit: 'contain',
  },
  footer: {
    ...shorthands.padding('0', tokens.spacingHorizontalM, tokens.spacingVerticalM),
  },
});

interface BookCardProps {
  book: Book;
}

export function BookCard({ book }: BookCardProps) {
  const styles = useStyles();

  // The locally stored cover keeps the page intact when the publisher host is unreachable.
  const coverSource = book.coverImage ?? book.coverUrl;

  return (
    <Card className={styles.card}>
      <CardPreview className={styles.preview}>
        <img className={styles.cover} src={coverSource} alt={`Cover of ${book.name}`} loading="lazy" />
      </CardPreview>

      <CardHeader
        header={<Text weight="semibold">{book.name}</Text>}
        description={<Caption1>{book.author}</Caption1>}
      />

      <div className={styles.footer}>
        <Link href={book.coverUrl} target="_blank" rel="noreferrer">
          Original cover
        </Link>
      </div>
    </Card>
  );
}
