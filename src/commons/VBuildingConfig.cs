using System.Collections.Generic;
using System.Linq;
using TUNING;

namespace AsLimc.commons {
    public abstract class VBuildingConfig : IBuildingConfig {
        public abstract LocString name { get; }
        public abstract LocString desc { get; }
        public abstract LocString effect { get; }
        public abstract string id { get; }

        /** TUNING.BUILDINGS.PLANORDER */
        public abstract string planName { get; }

        /** Database.Techs.Init */
        public abstract string techId { get; }

        protected abstract string anim { get; }
        protected abstract int width { get; }
        protected abstract int height { get; }
        protected abstract Dictionary<string, float> constructionRecipe { get; }
        private string[] constructionMaterials => constructionRecipe.Keys.ToArray();
        private float[] constructionMass => constructionRecipe.Values.ToArray();
        protected virtual int hitpoints => BUILDINGS.HITPOINTS.TIER0;
        protected virtual float constructionTime => BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER0;
        protected virtual float meltingPoint => BUILDINGS.MELTING_POINT_KELVIN.TIER0;
        protected virtual float temperatureModificationMassScale => BUILDINGS.MASS_TEMPERATURE_SCALE;
        protected virtual BuildLocationRule buildLocationRule => BuildLocationRule.Anywhere;
        protected virtual EffectorValues decor => BUILDINGS.DECOR.PENALTY.TIER0;
        protected virtual EffectorValues noise => NOISE_POLLUTION.NOISY.TIER0;

        /** OverlayScreen */
        protected virtual HashSet<Tag> overlayTags => null;

        /** OverlayModes */
        protected virtual HashedString viewMode => OverlayModes.None.ID;

        /**
         * minimal 
         */
        public sealed override BuildingDef CreateBuildingDef() {
            var buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim,
                hitpoints, constructionTime, constructionMass, constructionMaterials, meltingPoint, buildLocationRule, decor, noise, temperatureModificationMassScale);

            if (overlayTags != null) {
                GeneratedBuildings.RegisterWithOverlay(overlayTags, id);
            }

            if (viewMode != null) {
                buildingDef.ViewMode = viewMode;
            }

            ConfigureBuildingDef(buildingDef);
            return buildingDef;
        }

        protected virtual void ConfigureBuildingDef(BuildingDef buildingDef) {
        }
    }
}