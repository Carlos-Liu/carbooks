import { Body1, Title2, makeStyles, tokens } from '@fluentui/react-components';

import { AppLink } from '../components/AppLink';

const useStyles = makeStyles({
  root: {
    display: 'flex',
    flexDirection: 'column',
    rowGap: tokens.spacingVerticalM,
    alignItems: 'flex-start',
  },
});

export function NotFoundPage() {
  const styles = useStyles();

  return (
    <section className={styles.root}>
      <Title2>Page not found</Title2>
      <Body1>The page you asked for does not exist.</Body1>
      <AppLink to="/">Back to categories</AppLink>
    </section>
  );
}
