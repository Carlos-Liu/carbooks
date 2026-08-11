import {
  Tag,
  TagPicker,
  TagPickerControl,
  TagPickerGroup,
  TagPickerInput,
  TagPickerList,
  TagPickerOption,
  makeStyles,
  tokens,
} from '@fluentui/react-components';
import type { TagPickerProps } from '@fluentui/react-components';

import type { Tag as CatalogTag } from '../api/types';
import { accentForTagId } from './TagChip';

const useStyles = makeStyles({
  chip: {
    backgroundColor: tokens.colorNeutralBackground1,
  },
  optionChip: {
    display: 'inline-flex',
    alignItems: 'center',
    padding: `${tokens.spacingVerticalXXS} ${tokens.spacingHorizontalS}`,
    borderRadius: tokens.borderRadiusMedium,
    backgroundColor: tokens.colorNeutralBackground1,
    fontSize: tokens.fontSizeBase200,
    lineHeight: tokens.lineHeightBase200,
  },
  listHeader: {
    padding: `${tokens.spacingVerticalS} ${tokens.spacingHorizontalM}`,
    fontWeight: tokens.fontWeightSemibold,
    color: tokens.colorNeutralForeground2,
  },
});

interface TagMultiSelectProps {
  tags: CatalogTag[];
  selectedTagIds: string[];
  onSelectedTagIdsChange: (tagIds: string[]) => void;
  placeholder?: string;
}

/** Multiselect tag picker with removable chips, matching common label-picker UX. */
export function TagMultiSelect({
  tags,
  selectedTagIds,
  onSelectedTagIdsChange,
  placeholder = 'Select tags',
}: TagMultiSelectProps) {
  const styles = useStyles();
  const tagsById = new Map(tags.map((tag) => [tag.id, tag]));
  const availableTags = tags.filter((tag) => !selectedTagIds.includes(tag.id));

  const onOptionSelect: TagPickerProps['onOptionSelect'] = (_event, data) => {
    if (data.value === 'no-options') {
      return;
    }

    onSelectedTagIdsChange(data.selectedOptions);
  };

  return (
    <TagPicker selectedOptions={selectedTagIds} onOptionSelect={onOptionSelect}>
      <TagPickerControl>
        <TagPickerGroup aria-label="Selected tags">
          {selectedTagIds.map((tagId) => {
            const tag = tagsById.get(tagId);
            if (!tag) {
              return null;
            }

            return (
              <Tag
                key={tag.id}
                value={tag.id}
                shape="rounded"
                className={styles.chip}
                style={{ border: `1px solid ${accentForTagId(tag.id)}` }}
              >
                {tag.name}
              </Tag>
            );
          })}
        </TagPickerGroup>
        <TagPickerInput placeholder={placeholder} aria-label="Select tags" />
      </TagPickerControl>
      <TagPickerList>
        <div className={styles.listHeader}>All tags</div>
        {availableTags.length > 0 ? (
          availableTags.map((tag) => (
            <TagPickerOption key={tag.id} value={tag.id} text={tag.name}>
              <span
                className={styles.optionChip}
                style={{ border: `1px solid ${accentForTagId(tag.id)}` }}
              >
                {tag.name}
              </span>
            </TagPickerOption>
          ))
        ) : (
          <TagPickerOption value="no-options" text="No tags left">
            No tags left
          </TagPickerOption>
        )}
      </TagPickerList>
    </TagPicker>
  );
}
