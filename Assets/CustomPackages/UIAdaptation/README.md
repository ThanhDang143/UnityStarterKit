# UIAdaptation

Auto scale UI to fit with screen resolution.

## How to use
- Add UIScaler into Canvas
- Add UISaveZone to RectTransform need to resize.
- Currently ignore save zone function only use with RectTransform has anchor min(0,0), max(1,1).
- Use symbol `AUTO_UIADAPTATION_VERTICAL` or `AUTO_UIADAPTATION_HORIZONTAL` to auto add UIScaler into Canvas while use SSSystem
