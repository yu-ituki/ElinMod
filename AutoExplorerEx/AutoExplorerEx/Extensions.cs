public static class Extensions
{
	public static Point GetDestinationPoint(this AIAct act)
	{
		TaskPoint taskPoint = (TaskPoint)(object)((act is TaskPoint) ? act : null);
		if (taskPoint != null)
		{
			return taskPoint.pos;
		}
		return act.GetDestination();
	}

	public static int RealDistance(this Point a, Point b)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		PathProgress path = new PathProgress();
		path.RequestPathImmediate(a, b, 0, false, -1);
		if (path.nodes.Count == 0)
		{
			return int.MaxValue;
		}
		return path.nodes.Count;
	}
}
