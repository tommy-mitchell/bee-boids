public class AlignToPixelGrid : DisableOnRuntime
{
    private void Update() => transform.position = BoidLibrary.GenericMethods.RoundTo(transform.position, BoidLibrary.Constants.PIXELS_PER_UNIT);
}