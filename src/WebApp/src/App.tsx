import { Caption1, Title1, makeStyles, tokens } from '@fluentui/react-components';
import { Route, Routes } from 'react-router';

import { CategoryBooksPage } from './pages/CategoryBooksPage';
import { HomePage } from './pages/HomePage';
import { NotFoundPage } from './pages/NotFoundPage';

const useStyles = makeStyles({
  shell: {
    minHeight: '100%',
    display: 'flex',
    flexDirection: 'column',
  },
  header: {
    padding: `${tokens.spacingVerticalXL} ${tokens.spacingHorizontalXXL}`,
    backgroundColor: tokens.colorNeutralBackground1,
    borderBottom: `1px solid ${tokens.colorNeutralStroke2}`,
  },
  main: {
    flexGrow: 1,
    width: '100%',
    maxWidth: '1080px',
    margin: '0 auto',
    padding: `${tokens.spacingVerticalXXL} ${tokens.spacingHorizontalXXL}`,
  },
});

export default function App() {
  const styles = useStyles();

  return (
    <div className={styles.shell}>
      <header className={styles.header}>
        <Title1>CarBooks</Title1>
        <Caption1> A small catalog of motoring books.</Caption1>
      </header>

      <main className={styles.main}>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/categories/:slug" element={<CategoryBooksPage />} />
          <Route path="*" element={<NotFoundPage />} />
        </Routes>
      </main>
    </div>
  );
}
