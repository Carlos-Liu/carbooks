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
import { TagChip } from './TagChip';

const useStyles = makeStyles({
  card: {
    width: '260px',
  },
  preview: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    minHeight: '260px',
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
  tags: {
    display: 'flex',
    flexWrap: 'wrap',
    gap: tokens.spacingHorizontalXS,
    ...shorthands.padding('0', tokens.spacingHorizontalM, tokens.spacingVerticalM),
  },
});

interface BookCardProps {
  book: Book;
}

export function BookCard({ book }: BookCardProps) {
  const styles = useStyles();

  // Prefer the locally stored cover when the publisher host is unreachable.
  const coverSource = book.coverImage ?? book.coverUrl ?? undefined;
  const tags = book.tags ?? [];

  return (
    <Card className={styles.card}>
      <CardPreview className={styles.preview}>
        {coverSource ? (
          <img className={styles.cover} src={coverSource} alt={`Cover of ${book.name}`} loading="lazy" />
        ) : (
          <Caption1>No cover</Caption1>
        )}
      </CardPreview>

      <CardHeader
        header={<Text weight="semibold">{book.name}</Text>}
        description={
          <Caption1>
            {book.author}
            {book.translator ? ` · tr. ${book.translator}` : ''}
            {book.publisher ? ` · ${book.publisher}` : ''}
            {book.publishedOn ? ` · ${book.publishedOn}` : ''}
            {book.isbn ? ` · ISBN ${book.isbn}` : ''}
          </Caption1>
        }
      />

      {tags.length > 0 ? (
        <div className={styles.tags} aria-label={`Tags for ${book.name}`}>
          {tags.map((tag) => (
            <TagChip key={tag.id} id={tag.id} name={tag.name} />
          ))}
        </div>
      ) : null}

      {book.recommendation ? (
        <div className={styles.footer}>
          <Caption1>{book.recommendation}</Caption1>
        </div>
      ) : null}

      {book.coverUrl ? (
        <div className={styles.footer}>
          <Link href={book.coverUrl} target="_blank" rel="noreferrer">
            Original cover
          </Link>
        </div>
      ) : null}
    </Card>
  );
}
