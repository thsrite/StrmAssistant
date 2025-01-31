using Emby.Web.GenericEdit;
using Emby.Web.GenericEdit.Common;
using MediaBrowser.Model.Attributes;
using MediaBrowser.Model.LocalizationAttributes;
using StrmAssistant.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace StrmAssistant
{
    public class ModOptions: EditableOptionsBase
    {
        [DisplayNameL("PluginOptions_ModOptions_Mod_Features", typeof(Resources))]
        public override string EditorTitle => Resources.PluginOptions_ModOptions_Mod_Features;

        [DisplayNameL("GeneralOptions_MergeMultiVersion_Merge_Multiple_Versions", typeof(Resources))]
        [DescriptionL("GeneralOptions_MergeMultiVersion_Auto_merge_multiple_versions_if_in_the_same_folder_", typeof(Resources))]
        [Required]
        [EnabledCondition(nameof(IsModSupported), SimpleCondition.IsTrue)]
        public bool MergeMultiVersion { get; set; } = false;

        [DisplayNameL("ModOptions_ChineseMovieDb_Chinese_MovieDb", typeof(Resources))]
        [DescriptionL("ModOptions_ChineseMovieDb_Optimize_MovieDb_for_Chinese_metadata__Default_is_OFF_", typeof(Resources))]
        [EnabledCondition(nameof(IsMovieDbPluginLoaded), SimpleCondition.IsTrue)]
        [Required]
        public bool ChineseMovieDb { get; set; } = false;

        [Browsable(false)]
        public IEnumerable<EditorSelectOption> LanguageList { get; set; }

        [DisplayNameL("ModOptions_FallbackLanguages_Fallback_Languages", typeof(Resources))]
        [DescriptionL("ModOptions_FallbackLanguages_Fallback_languages__Default_is_zh_SG_", typeof(Resources))]
        [EditMultilSelect]
        [SelectItemsSource(nameof(LanguageList))]
        [VisibleCondition(nameof(ChineseMovieDb), SimpleCondition.IsTrue)]
        public string FallbackLanguages { get; set; }

        [DisplayNameL("ModOptions_OriginalPoster_Original_Poster", typeof(Resources))]
        [DescriptionL("ModOptions_OriginalPoster_Show_original_poster_based_on_original_language__Default_is_OFF_", typeof(Resources))]
        [Required]
        [EnabledCondition(nameof(IsModSupported), SimpleCondition.IsTrue)]
        public bool PreferOriginalPoster { get; set; } = false;

        [DisplayNameL("ModOptions_ExclusiveExtract_Exclusive_Extract", typeof(Resources))]
        [DescriptionL("ModOptions_ExclusiveExtract_Only_allow_this_plugin_to_extract_media_info__ffprobe__and_capture_image__ffmpeg___Default_is_OFF_", typeof(Resources))]
        [Required]
        [EnabledCondition(nameof(IsModSupported), SimpleCondition.IsTrue)]
        public bool ExclusiveExtract { get; set; } = false;
        
        [DisplayNameL("ModOptions_PinyinSortName_Pinyin_Sort_Title", typeof(Resources))]
        [DescriptionL("ModOptions_PinyinSortName_Auto_generate_pinyin_initials_as_sort_title", typeof(Resources))]
        [EnabledCondition(nameof(IsModSupported), SimpleCondition.IsTrue)]
        public bool PinyinSortName { get; set; } = false;

        [DisplayNameL("ModOptions_EnhanceChineseSearch_Enhance_Chinese_Search", typeof(Resources))]
        [DescriptionL("ModOptions_EnhanceChineseSearch_Support_Chinese_fuzzy_search_and_Pinyin_search__Default_is_OFF_", typeof(Resources))]
        [Required]
        [EnabledCondition(nameof(IsChineseSearchSupported), SimpleCondition.IsTrue)]
        public bool EnhanceChineseSearch { get; set; } = false;

        [Browsable(false)]
        public bool EnhanceChineseSearchRestore { get; set; } = false;
        
        public enum SearchItemType
        {
            [DescriptionL("ItemType_Movie_Movie", typeof(Resources))] Movie,
            [DescriptionL("ItemType_Collection_Collection", typeof(Resources))] Collection,
            [DescriptionL("ItemType_Series_Series", typeof(Resources))] Series,
            [DescriptionL("ItemType_Episode_Episode", typeof(Resources))] Episode,
            [DescriptionL("ItemType_Person_Person", typeof(Resources))] Person,
            [DescriptionL("ItemType_LiveTv_LiveTv", typeof(Resources))] LiveTv,
            [DescriptionL("ItemType_Music_Music", typeof(Resources))] Music,
            [DescriptionL("ItemType_MusicAlbum_MusicAlbum", typeof(Resources))] MusicAlbum,
            [DescriptionL("ItemType_Playlist_Playlist", typeof(Resources))] Playlist,
            [DescriptionL("ItemType_Photo_Photo", typeof(Resources))] Photo,
            [DescriptionL("ItemType_PhotoAlbum_PhotoAlbum", typeof(Resources))] PhotoAlbum,
            [DescriptionL("ItemType_Genre_Genre", typeof(Resources))] Genre,
            [DescriptionL("ItemType_Book_Book", typeof(Resources))] Book,
            [DescriptionL("ItemType_Game_Game", typeof(Resources))] Game,
            [DescriptionL("ItemType_Trailer_Trailer", typeof(Resources))] Trailer,
            [DescriptionL("ItemType_Tag_Tag", typeof(Resources))] Tag,
            [DescriptionL("ItemType_Studio_Studio", typeof(Resources))] Studio
        }

        [Browsable(false)]
        public IEnumerable<EditorSelectOption> SearchItemTypeList { get; set; }

        [DisplayNameL("ModOptions_SearchScope_Search_Scope", typeof(Resources))]
        [DescriptionL("ModOptions_SearchScope_Include_item_types__Blank_includes_all_", typeof(Resources))]
        [EditMultilSelect]
        [SelectItemsSource(nameof(SearchItemTypeList))]
        [VisibleCondition(nameof(EnhanceChineseSearch), SimpleCondition.IsTrue)]
        public string SearchScope { get; set; } =
            string.Join(",", new[] { SearchItemType.Movie, SearchItemType.Collection, SearchItemType.Series });

        [Browsable(false)]
        public bool IsMovieDbPluginLoaded { get; } =
            AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetName().Name == "MovieDb") &&
            RuntimeInformation.ProcessArchitecture == Architecture.X64;

        [Browsable(false)]
        public bool IsModSupported { get; } = RuntimeInformation.ProcessArchitecture == Architecture.X64;

        [Browsable(false)]
        public bool IsChineseSearchSupported { get; } = RuntimeInformation.ProcessArchitecture == Architecture.X64 &&
                                                        (Plugin.Instance.ApplicationHost.ApplicationVersion >=
                                                         new Version("4.8.3.0") &&
                                                         Plugin.Instance.ApplicationHost.ApplicationVersion <
                                                         new Version("4.9.0.0") ||
                                                         Plugin.Instance.ApplicationHost.ApplicationVersion ==
                                                         new Version("4.9.0.30"));
    }
}
