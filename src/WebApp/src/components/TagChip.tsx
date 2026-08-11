import { makeStyles, tokens } from '@fluentui/react-components';

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

/** Read-only tag chip used on the tags listing page. */
export function TagChip({ id, name }: TagChipProps) {
  const styles = useStyles();

  return (
    <span className={styles.chip} style={{ border: `1px solid ${accentForTagId(id)}` }}>
      {name}
    </span>
  );
}
