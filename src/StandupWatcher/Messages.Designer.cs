﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StandupWatcher {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("StandupWatcher.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Добавлен в список отслеживаемых стендаперов.
        /// </summary>
        internal static string AddedToFavoriteMessage {
            get {
                return ResourceManager.GetString("AddedToFavoriteMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Не указан автор.
        /// </summary>
        internal static string EmptyAddedToFavoriteMessage {
            get {
                return ResourceManager.GetString("EmptyAddedToFavoriteMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Ни за кем не следишь.
        /// </summary>
        internal static string EmptyFollowingStandupersMessage {
            get {
                return ResourceManager.GetString("EmptyFollowingStandupersMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Следишь за.
        /// </summary>
        internal static string FollowingStandupersMessage {
            get {
                return ResourceManager.GetString("FollowingStandupersMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Привет! Новый ивент в StandupStore - &lt;b&gt;{0}&lt;/b&gt;.
        ///
        ///Дата: {1}
        ///Ссылка: {2}
        ///
        ///Успей купить билет!.
        /// </summary>
        internal static string NewEventNotification {
            get {
                return ResourceManager.GetString("NewEventNotification", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Отсутствует в списке отслеживаемых.
        /// </summary>
        internal static string NotFoundInFavoriteMessage {
            get {
                return ResourceManager.GetString("NotFoundInFavoriteMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Не подписан.
        /// </summary>
        internal static string NotSubscribedStatusMessage {
            get {
                return ResourceManager.GetString("NotSubscribedStatusMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Удален из списка отслеживаемых стендаперов.
        /// </summary>
        internal static string RemovedFromFavoriteMessage {
            get {
                return ResourceManager.GetString("RemovedFromFavoriteMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Подписан.
        /// </summary>
        internal static string SubscribedStatusMessage {
            get {
                return ResourceManager.GetString("SubscribedStatusMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Рахмет братан.
        /// </summary>
        internal static string SuccessfulOperationMessage {
            get {
                return ResourceManager.GetString("SuccessfulOperationMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Не, такого не знаю.
        /// </summary>
        internal static string UnknownMessage {
            get {
                return ResourceManager.GetString("UnknownMessage", resourceCulture);
            }
        }
    }
}
