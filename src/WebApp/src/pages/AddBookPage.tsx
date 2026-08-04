import {
  Body1,
  Button,
  Field,
  Input,
  MessageBar,
  MessageBarBody,
  MessageBarTitle,
  Title2,
  makeStyles,
  tokens,
} from '@fluentui/react-components';
import { useMutation } from '@tanstack/react-query';
import { type FormEvent, useEffect, useState } from 'react';
import { useNavigate } from 'react-router';

import { ApiError } from '../api/client';
import { createBook } from '../api/catalog';
import { AppLink } from '../components/AppLink';

const useStyles = makeStyles({
  header: {
    display: 'flex',
    flexDirection: 'column',
    rowGap: tokens.spacingVerticalXS,
    marginBottom: tokens.spacingVerticalXL,
  },
  form: {
    display: 'flex',
    flexDirection: 'column',
    rowGap: tokens.spacingVerticalL,
    maxWidth: '480px',
  },
  actions: {
    display: 'flex',
    columnGap: tokens.spacingHorizontalM,
    alignItems: 'center',
  },
  preview: {
    maxWidth: '200px',
    maxHeight: '260px',
    objectFit: 'contain',
    backgroundColor: tokens.colorNeutralBackground3,
  },
});

export function AddBookPage() {
  const styles = useStyles();
  const navigate = useNavigate();

  const [name, setName] = useState('');
  const [author, setAuthor] = useState('');
  const [coverUrl, setCoverUrl] = useState('');
  const [coverImage, setCoverImage] = useState<File | null>(null);
  const [previewUrl, setPreviewUrl] = useState<string | null>(null);

  useEffect(() => {
    return () => {
      if (previewUrl) {
        URL.revokeObjectURL(previewUrl);
      }
    };
  }, [previewUrl]);

  const mutation = useMutation({
    mutationFn: (input: Parameters<typeof createBook>[0]) => createBook(input),
    onSuccess: () => {
      void navigate('/');
    },
  });

  const onCoverImageChange = (fileList: FileList | null) => {
    const file = fileList?.[0] ?? null;
    setCoverImage(file);

    if (previewUrl) {
      URL.revokeObjectURL(previewUrl);
    }

    setPreviewUrl(file ? URL.createObjectURL(file) : null);
  };

  const onSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    mutation.mutate({
      name: name.trim(),
      author: author.trim(),
      coverUrl: coverUrl.trim(),
      coverImage,
    });
  };

  const errorMessage =
    mutation.error instanceof ApiError || mutation.error instanceof Error
      ? mutation.error.message
      : mutation.error
        ? 'Could not create the book.'
        : null;

  return (
    <section>
      <div className={styles.header}>
        <AppLink to="/">Back to categories</AppLink>
        <Title2>Add a book</Title2>
        <Body1>Create a catalog entry and optionally upload a local cover image.</Body1>
      </div>

      <form className={styles.form} onSubmit={onSubmit}>
        <Field label="Name" required>
          <Input value={name} onChange={(_, data) => setName(data.value)} maxLength={64} required />
        </Field>

        <Field label="Author" required>
          <Input value={author} onChange={(_, data) => setAuthor(data.value)} maxLength={64} required />
        </Field>

        <Field label="Cover URL" required hint="Absolute http(s) URL of the publisher artwork.">
          <Input
            type="url"
            value={coverUrl}
            onChange={(_, data) => setCoverUrl(data.value)}
            maxLength={2048}
            required
          />
        </Field>

        <Field label="Cover image" hint="Optional. JPEG, PNG, GIF, WebP or SVG, up to 5 MB.">
          <input
            type="file"
            accept="image/jpeg,image/png,image/gif,image/webp,image/svg+xml"
            onChange={(event) => onCoverImageChange(event.target.files)}
          />
        </Field>

        {previewUrl ? (
          <img className={styles.preview} src={previewUrl} alt="Selected cover preview" />
        ) : null}

        {errorMessage ? (
          <MessageBar intent="error">
            <MessageBarBody>
              <MessageBarTitle>Could not add the book</MessageBarTitle>
              {errorMessage}
            </MessageBarBody>
          </MessageBar>
        ) : null}

        <div className={styles.actions}>
          <Button type="submit" appearance="primary" disabled={mutation.isPending}>
            {mutation.isPending ? 'Saving…' : 'Add book'}
          </Button>
          <Button type="button" appearance="secondary" onClick={() => void navigate('/')}>
            Cancel
          </Button>
        </div>
      </form>
    </section>
  );
}
