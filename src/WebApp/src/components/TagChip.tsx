import { makeStyles, tokens } from '@fluentui/react-components';
import { useHref, useNavigate } from 'react-router';

const tagAccentBorders = [
  '#c45c8a',
  '#c9a227',
  '#6b7280',
  '#3b82f6',
  '#0d9488',
  '#ea580c',
] as const;

const useStyles = makeStyles({
  chip: {
    display: 'inline-flex',
    alignItems: 'center',
    padding: `${tokens.spacingVerticalXXS} ${tokens.spacingHorizontalS}`,
    borderRadius: tokens.borderRadiusMedium,
    backgroundColor: tokens.colorNeutralBackground1,
    fontSize: tokens.fontSizeBase300,
    lineHeight: tokens.lineHeightBase300,
    color: 'inherit',
    textDecorationLine: 'none',
    cursor: 'pointer',
  },
});

export function accentForTagId(id: string): string {
  let hash = 0;
  for (let index = 0; index < id.length; index += 1) {
    hash = (hash + id.charCodeAt(index) * (index + 1)) % tagAccentBorders.length;
  }
  return tagAccentBorders[hash] ?? tagAccentBorders[0];
}

interface TagChipProps {
  id: string;
  name: string;
}

/** Clickable tag chip that navigates to the books labeled with this tag. */
export function TagChip({ id, name }: TagChipProps) {
  const styles = useStyles();
  const to = `/tags/${encodeURIComponent(id)}`;
  const href = useHref(to);
  const navigate = useNavigate();

  return (
    <a
      className={styles.chip}
      href={href}
      style={{ border: `1px solid ${accentForTagId(id)}` }}
      onClick={(event) => {
        const isModifiedClick =
          event.metaKey || event.ctrlKey || event.shiftKey || event.altKey || event.button !== 0;

        if (event.defaultPrevented || isModifiedClick) {
          return;
        }

        event.preventDefault();
        void navigate(to);
      }}
    >
      {name}
    </a>
  );
}
